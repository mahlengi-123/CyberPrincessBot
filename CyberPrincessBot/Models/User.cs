using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSafeBot.Models
{
    internal class User
    {
        public string Name { get; set; } = string.Empty;
        public string FavoriteTopic { get; set; } = string.Empty;
        public string LastSentiment { get; set; } = "neutral";
    }
}
