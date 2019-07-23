namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Test")]
    public partial class Test
    {
        public int ID { get; set; }

        public int ID_Client { get; set; }

        public int Money { get; set; }

        public virtual Client Client { get; set; }
    }
}
