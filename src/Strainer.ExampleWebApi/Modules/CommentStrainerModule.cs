using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.ExampleWebApi.Modules
{
    public class CommentStrainerModule : StrainerModule<Comment>
    {
        public override void Load(IStrainerModuleBuilder<Comment> builder)
        {
            builder
                .AddProperty(p => p.Id)
                .IsFilterable()
                .IsSortable()
                .IsDefaultSort();

            builder
                .AddProperty(p => p.Message)
                .IsFilterable()
                .IsSortable();

            builder
                .AddProperty(p => p.PostId)
                .IsFilterable()
                .IsSortable();
        }
    }
}
