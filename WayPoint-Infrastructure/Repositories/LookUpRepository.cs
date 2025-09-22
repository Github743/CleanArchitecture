using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WayPoint.Model;
using WayPoint_Infrastructure.Data;
using WayPoint_Infrastructure.Interfaces;

namespace WayPoint_Infrastructure.Repositories
{
    public class LookUpRepository : ILookUpRepository
    {
        private readonly ISqlEngine _sql;

        public LookUpRepository(ISqlEngine sql)
        {
            _sql = sql ?? throw new ArgumentNullException(nameof(sql));
        }
        public async Task<IReadOnlyList<Lookup>> GetLookupsByTypeName(string lookupTypebyName, CancellationToken ct = default)
        {
            {
                // null/empty default result
                var empty = Array.Empty<Lookup>();
                //lookupTypebyName = "ProductGroupType";
                //lookupTypebyName = "DiscountType";
                // fetch agreement (pass anonymous object for parameter)
                var lookups = await _sql.RetrieveObjectsAsync<Lookup>(
                    new { LookupTypeName = lookupTypebyName },
                    ct);

                // If none found, return empty list
                if (lookups == null)
                    return empty;


                // if the engine returns null (defensive), return empty
                return lookups ?? empty;
            }
        }
    }
}
