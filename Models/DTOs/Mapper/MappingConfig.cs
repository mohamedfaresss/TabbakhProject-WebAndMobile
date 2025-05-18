using AutoMapper;
using Models.Domain;
using Models.ArabicDomain;
using Models.DTOs.Food;
using Models.DTOs.Food.Arabic;
using Models.DTOs.User;
using System.Linq;

namespace Models.DTOs.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            // تعيينات اللغة الإنجليزية
            CreateMap<Recipe, RecipeWithNutritionDTO>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Recipe_Id))
                .ForMember(dest => dest.Recipe_Name, opt => opt.MapFrom(src => src.Recipe_Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Preparation_Method, opt => opt.MapFrom(src => src.Preparation_Method))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
                .ForMember(dest => dest.ImgaeUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Calories_100g, opt => opt.MapFrom(src => src.Nutrition.Calories_100g))
                .ForMember(dest => dest.Fat_100g, opt => opt.MapFrom(src => src.Nutrition.Fat_100g))
                .ForMember(dest => dest.Sugar_100g, opt => opt.MapFrom(src => src.Nutrition.Sugar_100g))
                .ForMember(dest => dest.Protein_100g, opt => opt.MapFrom(src => src.Nutrition.Protein_100g))
                .ForMember(dest => dest.Carb_100, opt => opt.MapFrom(src => src.Nutrition.Carb_100))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Nutrition.Type))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Recipe_Ingredient
                    .Select(ri => new IngredientAmountDTO
                    {
                        IngredientName = ri.Ingredient!.Ingredient_Name,
                        Amount = ri.Amount // افترضنا Amount هي double?
                    }).ToList()))
                .ForMember(dest => dest.IngredientNames, opt => opt.MapFrom(src => src.Recipe_Ingredient
                    .Select(ri => ri.Ingredient!.Ingredient_Name).ToList()));

            // تعيينات اللغة العربية
            CreateMap<الوصفات, وصفة_مع_تغذية_DTO>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.بطاقة_تعريف))
                .ForMember(dest => dest.Recipe_Name, opt => opt.MapFrom(src => src.اسم))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.وصف))
                .ForMember(dest => dest.Preparation_Method, opt => opt.MapFrom(src => src.طريقة_التحضير))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.الوقت))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.رابط_الصورة))
                .ForMember(dest => dest.Calories_100g, opt => opt.MapFrom(src => src.التغذية.سعرات_حرارية_لكل100جم))
                .ForMember(dest => dest.Fat_100g, opt => opt.MapFrom(src => src.التغذية.دهون_لكل100جم))
                .ForMember(dest => dest.Sugar_100g, opt => opt.MapFrom(src => src.التغذية.سكر_لكل100جم))
                .ForMember(dest => dest.Protein_100g, opt => opt.MapFrom(src => src.التغذية.بروتين_لكل100جم))
                .ForMember(dest => dest.Carb_100, opt => opt.MapFrom(src => src.التغذية.كربوهيدرات_لكل100جم))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.التغذية.النوع))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.وصفة_المكونات
                    .Select(ri => new IngredientAmountDTO
                    {
                        IngredientName = ri.المكون!.اسم_المكون,
                        Amount = ParseAmount(ri.كمية)
                    }).ToList()))
                .ForMember(dest => dest.IngredientNames, opt => opt.MapFrom(src => src.وصفة_المكونات
                    .Select(ri => ri.المكون!.اسم_المكون).ToList()));

        }

        private static double? ParseAmount(string amount)
        {
            if (string.IsNullOrWhiteSpace(amount))
                return null;

            // استخرج الرقم من النص (مثل "100 غرام" أو "100")
            var numberPart = amount.Split(' ')[0];
            if (double.TryParse(numberPart, out var result))
                return result;

            return null;
        }
    }
}