using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalConcepts
{
    public class User
    {
        public bool IsAdministrator { get; }
        public bool CanWrite { get; }
        public bool CanRead { get; }

        public static bool CanAccessPage(User user, Page page)
        {
            if (page.RequiresAdministrator && user.IsAdministrator)
            {
                return true;
            }

            if (page.RequiresSuperUser && (user.IsAdministrator || user.CanWrite))
            {
                return true;
            }

            if (user.IsAdministrator || user.CanWrite || user.CanRead)
            {
                return true;
            }

            return false;
        }

        public static bool CanAccessPage2(User user, Page page)
        {
            if (user.IsAdministrator)
            {
                return true;
            }

            if (page.RequiresSuperUser && user.CanWrite)
            {
                return true;
            }

            if (user.CanWrite || user.CanRead)
            {
                return true;
            }

            return false;
        }

        private void CheckInvariants()
        {
            VerifyIfThen(CanWrite, CanRead,
                "if the user can writen then the user can read");
        }

        private void VerifyIfThen(bool condition, bool inference, string invariant)
        {
            if (condition == true && inference == false)
            {
                throw new InvalidOperationException($"Invariant vioated: {invariant}.");
            }
        }
    }
}
