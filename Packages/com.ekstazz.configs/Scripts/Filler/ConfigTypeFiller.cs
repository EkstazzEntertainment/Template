namespace Ekstazz.Configs
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    
    
    internal class ConfigTypeFiller
    {
        internal ConfigPropertyFillerFactory FillerFactory { get; set; }
        internal PostProcessorsFactory PostProcessorsFactory { get; set; }
        
        private readonly List<PropertyInfo> propertiesList = new List<PropertyInfo>();
        
        
        public virtual void Fill(object classToBeFilled)
        {
            propertiesList.Clear();
            if (classToBeFilled == null)
            {
                throw new ArgumentNullException(nameof(classToBeFilled));
            }

            var type = classToBeFilled.GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in properties.Where(info => info.GetCustomAttribute<ConfigPropertyAttribute>() != null))
            {
                Add(propertyInfo);
            }

            foreach (var property in propertiesList)
            {
                FillPropertyOf(classToBeFilled, property);
            }

            foreach (var property in propertiesList.OrderByDescending(OrderInAttributes))
            {
                PostProcess(classToBeFilled, property);
            }

            if (classToBeFilled is IPostProcessable processable)
            {
                processable.PostProcess();
            }
        }
        
        private void PostProcess(object classToBeFilled, PropertyInfo property)
        {
            try
            {
                var attribute = property.GetCustomAttribute<ConfigPropertyAttribute>(true);
                var postProcessor = PostProcessorsFactory.Create(attribute.PostProcessor);
                postProcessor.Process(classToBeFilled, property);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private int OrderInAttributes(PropertyInfo pi)
        {
            return pi.GetCustomAttribute<ConfigPropertyAttribute>(true).Priority;
        }

        private void FillPropertyOf(object classToBeFilled, PropertyInfo property)
        {
            try
            {
                var propertyFiller = FillerFactory.CreateFillerFor(property);
                var attribute = property.GetCustomAttribute<ConfigPropertyAttribute>();
                propertyFiller.Fill(classToBeFilled, property, GetPropertyKey(property, attribute), attribute.IsMultiConfig);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static string GetPropertyKey(PropertyInfo property, ConfigPropertyAttribute attribute)
        {
            return string.IsNullOrEmpty(attribute.Key) ? property.Name : attribute.Key;
        }

        protected virtual void Add(PropertyInfo propertyInfo)
        {
            propertiesList.Add(propertyInfo);
        }
    }
}