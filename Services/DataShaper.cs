using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T>
        where T : class
    {

        public PropertyInfo[] Properties  { get; set; }

        public DataShaper()
        {
            Properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance ) ;
        }
        //ExpandoObject çalışma zamanında herhangi bir nesneye değmektedir.
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> shapes, string fieldsString)
        {
            var requiredField = GetRequiredProperties(fieldsString) ;   
            return FetchData(shapes, requiredField) ;
        }

        public ShapedEntity ShapeData(T shape, string fieldsString)
        {
            var requiredProperties  = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(shape, requiredProperties);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredFields = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(',',StringSplitOptions.RemoveEmptyEntries);
                foreach (var  field in fields)
                {
                    var property = Properties.FirstOrDefault(
                        p => p.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property is null)
                    {
                        continue;
                    }
                    requiredFields.Add(property);

                }

            }
            else
            {
                requiredFields = Properties.ToList();
            }

            return requiredFields; 

        }



        private ShapedEntity FetchDataForEntity (T shape  ,IEnumerable<PropertyInfo> requiredProperties)
        {

            var shapedObject = new ShapedEntity();

            foreach (var property in requiredProperties )
            {
                var objectPropertyValues= property.GetValue(shape);
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValues);
            }

            var objectProperty = shape.GetType().GetProperty("Id");
            shapedObject.Id = (int)objectProperty.GetValue(shape);
            return shapedObject;
        }
        private IEnumerable<ShapedEntity> FetchData (IEnumerable<T> entities , IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ShapedEntity>();

            foreach (var property in entities)
            {
                var shapedObject = FetchDataForEntity(property , requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        } 
    }
}
