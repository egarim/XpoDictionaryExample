using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpoDictionaryExample.Module.BusinessObjects
{
    [NonPersistent]
    public class BasePersistentClass : XPLiteObject
    {
        public BasePersistentClass(Session session) : base(session) { }
        public BasePersistentClass(Session session, XPClassInfo classInfo) : base(session, classInfo) { }
    }
}
