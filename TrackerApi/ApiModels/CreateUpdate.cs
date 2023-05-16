using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TrackerApi.ApiModels
{
    public class CreateUpdate<T>
    {
        public bool Success { get; set; }

        public T Model { get; set; }

        public ModelStateDictionary Errors { get; set; }
    }
}