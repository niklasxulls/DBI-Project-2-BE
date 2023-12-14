using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace stackblob.Infrastructure.Persistence.Extensions;
public static class EntityTypeBuilderExtension { 
    public static void Json<TEntity, TChildEntity>(EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, IProperty>> propertyExpression)
        where TEntity : Json<TChildEntity>
    {
        //Activator.CreateInstance(typeof(Json<TChildEntity>));
        //builder.Property(propertyExpression).HasConversion(
        //       from =>  ((Json<TChildEntity>) from).Serialized,
        //       (var to) 
        //       {
        //           var objc = (Json<TChildEntity>) Activator.CreateInstance(typeof(Json<TChildEntity>));
        //           objc.Serialized = to;
        //           return (IProperty) objc;
        //       }
               
        //);
    }
}
