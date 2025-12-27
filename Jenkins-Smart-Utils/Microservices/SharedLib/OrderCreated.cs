using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib;

public record OrderCreated(int OrderId, string ProductName, int Count, decimal TotalPrice, DateTime CreatedOn);

