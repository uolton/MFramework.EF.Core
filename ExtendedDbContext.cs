using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MFramework.Common.Core.Collections.Extensions;
using MFramework.EF.Core.Builder;
using MFramework.EF.Core.Conventions;
using MFramework.EF.Core.Conventions.Metadata;
using MFramework.EF.Core.Defaults;
using MFramework.EF.Core.Indexes;
using MFramework.EF.Core.Pluralization;
using Extensions = MFramework.Common.Core.Collections.Extensions.CollectionExtensions;


namespace MFramework.EF.Core
{
	/// <summary>
	/// Extended context class that supports functionality in the project
	/// </summary>
	public abstract class ExtendedDbContext : DbContext
	{
		// ReSharper disable PublicConstructorInAbstractClass
		/// <summary>
		/// Creates new instance based on connection string
		/// </summary>
		/// <param name="connectionString">Connection string to the context</param>
		public ExtendedDbContext(string connectionString) :
			// ReSharper restore PublicConstructorInAbstractClass
			base(connectionString)
		{

		}
		/// <summary>
		/// Build out the model
		/// </summary>
		/// <param name="modelBuilder">Mode builder for the context</param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

			// call derived class to add conventions
			AddConventions();
			// now process conventions
			ProcessAddedConventions(modelBuilder);
		}

		/// <summary>
		/// Force implementation via abstract class
		/// </summary>
		protected abstract void AddConventions();

		// conventions saved here
		private readonly List<IConvention> _conventions = new List<IConvention>();
        /// <summary>
        /// Force implementation via abstract class
        /// </summary>
        
        

		//reflection data about DbContext, its sets, properties and attributes
		private static readonly Dictionary<string, List<DbSetMetadata>> DbSetMetadata =
			new Dictionary<string, List<DbSetMetadata>>();

		private static readonly object Locker = new object();

		/// <summary>
		/// Add one convention
		/// </summary>
		/// <param name="convention">Convention to add</param>
		protected void AddConvention(IConvention convention)
		{
			_conventions.Add(convention);
		}
        

        

        /// <summary>
		/// Process all added conventions
		/// </summary>
		/// <param name="modelBuilder">Model builder</param>
		protected virtual void ProcessAddedConventions(DbModelBuilder modelBuilder)
		{
			if (_conventions.Count > 0)
			{
				// populate reflection data
				PopulateSetMetadata();
				// run through all global added conventions
				_conventions.ForEach(convention =>
				{
					if (convention is IGlobalConvention)
					{
						ProcessGlobalConvention(modelBuilder, convention as IGlobalConvention);
					}
				});
				// run through attribute based conventions
				_conventions.ForEach(convention =>
				{
					if (convention is IAttributeConvention)
					{
						ProcessAttributeBasedConvention(modelBuilder, convention as IAttributeConvention);
					}
				});
			}
		}


		/// <summary>
		/// Process global conventions
		/// </summary>
		/// <param name="modelBuilder">Model builder</param>
		/// <param name="convention">One global convention to process</param>
		private void ProcessGlobalConvention(DbModelBuilder modelBuilder, IGlobalConvention convention)
		{
			var key = GetType().AssemblyQualifiedName;
			Debug.Assert(key != null, "key != null");
			if (DbSetMetadata.ContainsKey(key))
			{
				var setMetadata = DbSetMetadata[key];
				// run through DbSets in current context
				setMetadata.ForEach(set => Extensions.ForEach(set.DbSetItemProperties, prop =>
					{
						// get type of property that matches current convention
						List<Type> targetTypes = GetMatchingTypeForConfiguration(convention.PropertyConfigurationType);
						// make sure this type matched property type
						if (targetTypes.Contains(prop.PropertyInfo.PropertyType))
						{
							// Get entity method in ModuleBuilder
							// we are trying to get to the point of expressing the following
							//modelBuilder.Entity<Person>().Property(a => a.Name).IsMaxLength() for example
							var setMethod = modelBuilder.GetType()
								.GetMethod("Entity", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
							// one we have Entity method, we have to add generic parameters to get to Entity<T>
							var genericSetMethod = setMethod.MakeGenericMethod(new[] { set.ItemType });
							// Get an instance of EntityTypeConfiguration<T>
							var entityInstance = genericSetMethod.Invoke(modelBuilder, null);

							//Get methods of EntityTypeConfiguration<T>
							var propertyAccessors = entityInstance.GetType().GetMethods(
								BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();

							// we are looking for Property method that returns PropertyConfiguration
							// that is used in current convention
							// we are looking for Property method that returns PropertyConfiguration
							// that is used in current convention
							bool isNullableProperty = false;
							// check for nullable property
							if (prop.PropertyInfo.PropertyType.IsGenericType)
							{
								var arguments = prop.PropertyInfo.PropertyType.GetGenericArguments();
								if (arguments.Length == 1 && prop.PropertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
								{
									isNullableProperty = true;
								}
							}
							MethodInfo propertyMethod = null;
							propertyAccessors.Where(
								oneProperty =>
									oneProperty.ReturnType == convention.PropertyConfigurationType).ToList().ForEach(
									one =>
									{
										if (isNullableProperty)
										{
											// nullable property will have generic type
											if (one.GetParameters()[0].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1].IsGenericType)
											{
												propertyMethod = one;
											}
										}
										else
										{
											// non nullable property is non-generic type
											if (!one.GetParameters()[0].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1].IsGenericType)
											{
												propertyMethod = one;
											}
										}
									});

							//Get method handle in order to build the expression 
							// example: (a => a.Name)
							var expressionGetMethod = GetPropertyExpressionMethodHandle();

							//Create lambda expression by making expression method that takes two generic parameters
							// one for class, the other for property type
							var genericExpressionMethod = expressionGetMethod
								.MakeGenericMethod(new[] { prop.PropertyInfo.ReflectedType, prop.PropertyInfo.PropertyType });

							//FInally, get lambda expression it self
							// example: (a => a.Name)
							var propertyExpression = genericExpressionMethod.Invoke(null, new object[] { prop.PropertyInfo });

							//Not get an instance of PrimitivePropertyConfiguration by invoking EntityTypeConfiguration<T>'s 
							// Property() method
							var config = propertyMethod
											 .Invoke(entityInstance, new[] { propertyExpression }) as PrimitivePropertyConfiguration;

							//Finally, pass this configuration and attribute into the convention
							convention.ApplyConfiguration(prop.PropertyInfo, config);
						}
					}));
			}
		}

		/// <summary>
		/// Determine what property type should be used for a specific convention
		/// </summary>
		/// <param name="propertyConfigurationType">
		/// Type of PrimitivePropertyConfiguration to process
		/// </param>
		/// <returns>
		/// Property types that should be used with current convention
		/// </returns>
		private List<Type> GetMatchingTypeForConfiguration(Type propertyConfigurationType)
		{
			var returnValue = new List<Type>();
			if (propertyConfigurationType == typeof(DecimalPropertyConfiguration))
			{
				returnValue.Add(typeof(decimal));
				returnValue.Add(typeof(decimal?));
			}
			if (propertyConfigurationType == typeof(StringPropertyConfiguration))
			{
				returnValue.Add(typeof(string));
			}
			if (propertyConfigurationType == typeof(DateTimePropertyConfiguration))
			{
				returnValue.Add(typeof(DateTime));
				returnValue.Add(typeof(DateTime?));
			}
			// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
			if (propertyConfigurationType == typeof(BinaryPropertyConfiguration))
			// ReSharper restore ConvertIfStatementToConditionalTernaryExpression
			{
				returnValue.Add(typeof(byte[]));
			}
			else
			{
				returnValue.Add(typeof(object));
			}
			return returnValue;
		}


		/// <summary>
		/// Process attribute based conventions
		/// </summary>
		/// <param name="modelBuilder">Model builder</param>
		/// <param name="convention">One attribute convention to process</param>
		private void ProcessAttributeBasedConvention(DbModelBuilder modelBuilder, IAttributeConvention convention)
		{
			var key = GetType().AssemblyQualifiedName;
			Debug.Assert(key != null, "key != null");

			if (DbSetMetadata.ContainsKey(key))
			{
				var setMetadata = DbSetMetadata[key];
				// run through DbSets in current context
				setMetadata.ForEach(
					set => set.DbSetItemProperties.ToList().ForEach(
						prop =>
						{
							// get attribute that matches convention
							var data = prop.DbSetItemAttributes
								.Where(attr => attr.Attribute.GetType() == convention.AttributeType).FirstOrDefault();

							// this class's property has the attribute
							if (data != null)
							{
								// Get entity method in ModuleBuilder
								// we are trying to get to the point of expressing the following
								//modelBuilder.Entity<Person>().Property(a => a.Name).IsMaxLength() for example
								var setMethod = modelBuilder.GetType()
									.GetMethod("Entity", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
								// one we have Entity method, we have to add generic parameters to get to Entity<T>
								var genericSetMethod = setMethod.MakeGenericMethod(new[] { set.ItemType });
								// Get an instance of EntityTypeConfiguration<T>
								var entityTypeConfigurationInstance = genericSetMethod.Invoke(modelBuilder, null);

								//Get methods of EntityTypeConfiguration<T>
								var propertyAccessors = entityTypeConfigurationInstance.GetType().GetMethods(
									BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();

								// we are looking for Property method that returns PropertyConfiguration
								// that is used in current convention
								bool isNullableProperty = false;
								// check for nullable property
								if (prop.PropertyInfo.PropertyType.IsGenericType)
								{
									var arguments = prop.PropertyInfo.PropertyType.GetGenericArguments();
									if (arguments.Length == 1 && prop.PropertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
									{
										isNullableProperty = true;
									}
								}
								MethodInfo propertyMethod = null;
								propertyAccessors.Where(
									oneProperty =>
										oneProperty.ReturnType == convention.PropertyConfigurationType).ToList().ForEach(
											one =>
											{
												if (isNullableProperty)
												{
													// nullable property will have generic type
													if (one.GetParameters()[0].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1].IsGenericType)
													{
														propertyMethod = one;
													}
												}
												else
												{
													// non nullable property is non-generic type
													if (!one.GetParameters()[0].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1].IsGenericType)
													{
														propertyMethod = one;
													}
												}
											});


								//Get method handle in order to build the expression 
								// example: (a => a.Name)
								var expressionGetMethod = GetPropertyExpressionMethodHandle();

								//Create lambda expression by making expression method that takes two generic parameters
								// one for class, the other for property type
								var genericExpressionMethod = expressionGetMethod
									.MakeGenericMethod(new[] { prop.PropertyInfo.ReflectedType, prop.PropertyInfo.PropertyType });

								//FInally, get lambda expression it self
								// example: (a => a.Name)
								var propertyExpression = genericExpressionMethod.Invoke(null, new object[] { prop.PropertyInfo });

								//Not get an instance of PrimitivePropertyConfiguration by invoking EntityTypeConfiguration<T>'s 
								// Property() method
								var config = propertyMethod
												 .Invoke(entityTypeConfigurationInstance, new[] { propertyExpression }) as PrimitivePropertyConfiguration;

								//Finally, pass this configuration and attribute into the convention
								convention.ApplyConfiguration(prop.PropertyInfo, config, data.Attribute);
							}

						}));
			}
		}

		/// <summary>
		/// Locate member info handle for GetPropertyExpression method by iterating through
		/// class hierarchy
		/// </summary>
		/// <returns>MemberInfo handle for GetPropertyExpression method</returns>
		private MethodInfo GetPropertyExpressionMethodHandle()
		{
			MethodInfo returnValue = null;
			Type currentType = GetType();
			while (returnValue == null)
			{
				returnValue = currentType
								.GetMethod("GetPropertyExpression",
								BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Static);
				if (returnValue == null)
				{
					currentType = currentType.BaseType;
					if (currentType == null)
					{
						break;
					}
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Create Expression that can access property on a class.  You would typically write it as 
		/// (p=>p.Name)
		/// In our case we are using Expression to build the same expression
		/// </summary>
		/// <typeparam name="TClass">Class type that is owning the property in question</typeparam>
		/// <typeparam name="TProperty">Property type</typeparam>
		/// <param name="property">PropertyInfo object for property in question</param>
		/// <returns>Expression that returns the property, such as (p=>p.Name)</returns>
		// ReSharper disable UnusedMember.Local
		private static Expression<Func<TClass, TProperty>> GetPropertyExpression<TClass, TProperty>(PropertyInfo property)
		// ReSharper restore UnusedMember.Local
		{
			//  Create {p=> portion of the Expression in example (p=>p.Name)
			var objectExpression = Expression.Parameter(property.ReflectedType, "parameter");
			// create property expression - .Name for example
			var propertyExpression = Expression.Property(objectExpression, property);
			//Create lambda expression from two parts
			var returnValue = Expression.Lambda<Func<TClass, TProperty>>(propertyExpression, objectExpression);
			return returnValue;
		}

		/// <summary>
		/// RUn through DbContnxt sets and save reflection data in a dictionary
		/// </summary>
		private void PopulateSetMetadata()
		{
			var key = GetType().AssemblyQualifiedName;
			Debug.Assert(key != null, "key != null");

			if (!DbSetMetadata.ContainsKey(key))
			{
				lock (Locker)
				{
					if (!DbSetMetadata.ContainsKey(key))
					{
						var props = GetType().GetProperties(
							BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();
						var sets = new List<DbSetMetadata>();
						props.ForEach(one =>
						{
							//Filter out db sets
							if (one.PropertyType.IsGenericType &&
								(one.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) ||
								one.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>)))
							{
								sets.Add(new DbSetMetadata(one.PropertyType.GetGenericArguments().First(), one));
							}
						});
						// add this context to dictionary
						DbSetMetadata.Add(key, sets);
					}
				}
			}
		}

		/// <summary>
		/// Get indexes collection from the DbContext
		/// </summary>
		/// <returns>Collection of defaults</returns>
		public IEnumerable<IndexInfo> GetIndexes()
		{
			PopulateSetMetadata();
			// ReSharper disable TooWideLocalVariableScope
			string columnName;
			string tableName;
			// ReSharper restore TooWideLocalVariableScope
			var key = GetType().AssemblyQualifiedName;
			var returnValue = new List<IndexInfo>();
			Debug.Assert(key != null, "key != null");
			DbSetMetadata[key].ForEach(
				one =>
				{
					// ReSharper disable ConvertToLambdaExpression
                    
					Extensions.ForEach(one.DbSetItemProperties, property =>
						{
							var indexedAttributes =
								Enumerable.Where<DbAttributeMetadata>(property.DbSetItemAttributes, attribute => attribute.Attribute.GetType().IsAssignableFrom(typeof(IndexedAttribute)));
							foreach (var indexedAttribute in indexedAttributes)
							{
								var attribute = indexedAttribute.Attribute as IndexedAttribute;
								Debug.Assert(attribute != null, "attribute != null");
								columnName = attribute.ColumnName;
								if (string.IsNullOrEmpty(columnName))
								{
									var columnAttribute =
									Enumerable.FirstOrDefault<DbAttributeMetadata>(property.DbSetItemAttributes, attr => attr.Attribute.GetType().IsAssignableFrom(typeof(ColumnAttribute)));
									columnName = columnAttribute != null ? ((ColumnAttribute)columnAttribute.Attribute).Name : property.PropertyInfo.Name;
								}
								tableName = attribute.TableName;
								if (string.IsNullOrEmpty(tableName))
								{
									var tableAttribute =
										one.DbSetItemAttributes.FirstOrDefault(
											attr => attr.Attribute.GetType().IsAssignableFrom(typeof(TableAttribute)));
									tableName = tableAttribute != null ? ((TableAttribute)tableAttribute.Attribute).Name : string.Empty;
									if (string.IsNullOrEmpty(tableName))
									{
										var pluraizedName = Pluralizer.Pluralize(one.ItemType.Name);
										returnValue.Add(
											new IndexInfo(
												attribute.IndexName,
												attribute.OrdinalPoistion,
												columnName,
												attribute.Direction,
												new string[] { pluraizedName, one.ItemType.Name }));
									}
									else
									{
										returnValue.Add(
											new IndexInfo(
												attribute.IndexName,
												attribute.OrdinalPoistion,
												columnName,
												attribute.Direction,
												tableName));
									}
								}
								else
								{
									returnValue.Add(
										new IndexInfo(
											attribute.IndexName,
											attribute.OrdinalPoistion,
											columnName,
											attribute.Direction,
											tableName));
								}

							}
						});
					// ReSharper restore ConvertToLambdaExpression
				});
			return returnValue;
		}

		/// <summary>
		/// Get defaults collection from the DbContext
		/// </summary>
		/// <returns>Collection of defaults</returns>
		public IEnumerable<DefaultInfo> GetDefaults()
		{
			PopulateSetMetadata();
			// ReSharper disable TooWideLocalVariableScope
			string columnName;
			string tableName;
			// ReSharper restore TooWideLocalVariableScope
			var key = GetType().AssemblyQualifiedName;
			var returnValue = new List<DefaultInfo>();
			Debug.Assert(key != null, "key != null");
			DbSetMetadata[key].ForEach(
				one =>
				{
					// ReSharper disable ConvertToLambdaExpression
					one.DbSetItemProperties.ForEach(property =>
						{
							var defaultAttribute =
								Enumerable.FirstOrDefault<DbAttributeMetadata>(property.DbSetItemAttributes, attribute => attribute.Attribute.GetType().IsAssignableFrom(typeof(DefaultAttribute)));
							if (defaultAttribute != null)
							{
								var attribute = defaultAttribute.Attribute as DefaultAttribute;
								Debug.Assert(attribute != null, "attribute != null");
								columnName = attribute.ColumnName;
								if (string.IsNullOrEmpty(columnName))
								{
									var columnAttribute =
									Enumerable.FirstOrDefault<DbAttributeMetadata>(property.DbSetItemAttributes, attr => attr.Attribute.GetType().IsAssignableFrom(typeof(ColumnAttribute)));
									columnName = columnAttribute != null ? ((ColumnAttribute)columnAttribute.Attribute).Name : property.PropertyInfo.Name;
								}
								tableName = attribute.TableName;
								if (string.IsNullOrEmpty(tableName))
								{
									var tableAttribute =
										one.DbSetItemAttributes.FirstOrDefault(
											attr => attr.Attribute.GetType().IsAssignableFrom(typeof(TableAttribute)));
									tableName = tableAttribute != null ? ((TableAttribute)tableAttribute.Attribute).Name : string.Empty;
									if (string.IsNullOrEmpty(tableName))
									{
										var pluraizedName = Pluralizer.Pluralize(one.ItemType.Name);
										returnValue.Add(
											new DefaultInfo(
												new string[] { pluraizedName, one.ItemType.Name },
												columnName,
												attribute.DefaultValueExpression));
									}
									else
									{
										returnValue.Add(
											new DefaultInfo(
												tableName,
												columnName,
												attribute.DefaultValueExpression));
									}
								}
								else
								{
									returnValue.Add(
										new DefaultInfo(
											tableName,
											columnName,
											attribute.DefaultValueExpression));
								}

							}
						});
					// ReSharper restore ConvertToLambdaExpression
				});
			return returnValue;
		}
	}

}
