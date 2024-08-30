﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesHub.Application.External
{
    public static class SD
    {
        // Roles
        public const string AdminRole = "Admin";
        public const string EditorRole = "Editor";
        public const string CustomerRole = "Customer";
        // ----------------------------
        public const string Pending = "Pending";
        public const string Approve = "Approve";
        public const string Proccessing = "Proccessing";
        public const string Canceled = "Canceled";
        public const string Shipped = "Shipped";
        public const string Refund = "Refund";
        public const string Rejected = "Rejected";

        public const string SessionKey = "ShoppingCartSession";

    }
}
