using System;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using BL = BlueBit.HR.Docs.BL;

namespace BlueBit.HR.Docs.WWW.Models
{
    public abstract class EntityModelBase :
        IValidatableObject
    {
        public abstract BL.DataLayer.Entities.Commons.IObjectInDBWithID SourceEntityWithID { get; }
        public abstract bool IsFromDB { get; }
        public abstract System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }

    public abstract class EntityModelBase<T> :
        EntityModelBase
        where T : BL.DataLayer.Entities.Commons.IObjectInDBWithID_Bindable, new()
    {
        public readonly T SourceEnity;
        public override sealed BL.DataLayer.Entities.Commons.IObjectInDBWithID SourceEntityWithID { get { return SourceEnity; } }
        public override sealed bool IsFromDB { get { return SourceEnity.ID > 0; } }

        [Key]
        [Display(Name = "#ID:")]
        public long ID { get { return SourceEnity.ID; } set { SourceEnity.ID = value;  } }

        public EntityModelBase()
        {
            this.SourceEnity = new T();
        }
        public EntityModelBase(T sourceEntity)
        {
            Contract.Requires(sourceEntity != null);
            this.SourceEnity = sourceEntity;
        }
    }
}