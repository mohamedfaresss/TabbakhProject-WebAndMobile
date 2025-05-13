// MappingConfig.cs
using AutoMapper;
using Models.Domain;
using Models.ArabicDomain;
using Models.DTOs.Food;
using Models.DTOs.Food.Arabic;
using Models.DTOs.User;

namespace Models.DTOs.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            CreateMap<Recipe, RecipeWithNutritionDTO>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Recipe_Id))
                .ForMember(dest => dest.Recipe_Name, opt => opt.MapFrom(src => src.Recipe_Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Preparation_Method, opt => opt.MapFrom(src => src.Preparation_Method))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
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
                        Amount = ri.Amount
                    }).ToList()));

            CreateMap<الوصفات, وصفة_مع_تغذية_DTO>()
                 .ForMember(dest => dest.بطاقة_تعريف, opt => opt.MapFrom(src => src.بطاقة_تعريف))
                 .ForMember(dest => dest.اسم_الوصفة, opt => opt.MapFrom(src => src.اسم))
                 .ForMember(dest => dest.الوصف, opt => opt.MapFrom(src => src.وصف))
                 .ForMember(dest => dest.طريقة_التحضير, opt => opt.MapFrom(src => src.طريقة_التحضير))
                 .ForMember(dest => dest.الوقت, opt => opt.MapFrom(src => src.الوقت))
                 .ForMember(dest => dest.سعرات_حرارية_لكل100ج, opt => opt.MapFrom(src => src.التغذية.سعرات_حرارية_لكل100جم))
                 .ForMember(dest => dest.دهون_لكل100ج, opt => opt.MapFrom(src => src.التغذية.دهون_لكل100جم))
                 .ForMember(dest => dest.سكر_لكل100ج, opt => opt.MapFrom(src => src.التغذية.سكر_لكل100جم))
                 .ForMember(dest => dest.بروتين_لكل100ج, opt => opt.MapFrom(src => src.التغذية.بروتين_لكل100جم))
                 .ForMember(dest => dest.كربوهيدرات_لكل100, opt => opt.MapFrom(src => src.التغذية.كربوهيدرات_لكل100جم))
                 .ForMember(dest => dest.النوع, opt => opt.MapFrom(src => src.التغذية.النوع))
                 .ForMember(dest => dest.المكونات, opt => opt.MapFrom(src => src.وصفة_المكونات
                     .Select(ri => new مكون_مع_الكمية_DTO
                     {
                         اسم_المكون = ri.المكون!.اسم_المكون,
                         الكمية = ri.كمية
                     }).ToList()));
        }
    }
}
      