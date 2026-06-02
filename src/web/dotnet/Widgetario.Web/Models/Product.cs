using System;

namespace Widgetario.Web.Models
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get;  set; }

        public string StockMessage
        {
            get
            {
                var message = "Plenty";
                if (Stock == 0)
                {
                    message = "SOLD OUT!";
                }
                else if (Stock < 50)
                {
                    message = "Last few...";
                }
                return message;
            }
        }

        public string DisplayPrice
        {
            get
            {
                return $"${Price.ToString("#.00")}";
            }
        }

        public string Emoji
        {
            get
            {
                var lower = (Name ?? string.Empty).ToLowerInvariant();
                if (lower.Contains("soc") || lower.Contains("cpu") || lower.Contains("chip")) return "🔌";
                if (lower.Contains("iot") || lower.Contains("breakout")) return "🤖";
                if (lower.Contains("dac") || lower.Contains("audio")) return "🎛";
                if (lower.Contains("comms") || lower.Contains("radio") || lower.Contains("mars")) return "📡";
                if (lower.Contains("board")) return "🧩";
                if (lower.Contains("sensor")) return "📟";
                return "📦";
            }
        }

        public string TileColor
        {
            get
            {
                var name = Name ?? string.Empty;
                int sum = 0;
                foreach (var c in name) sum += (int)c;
                int idx = (Math.Abs(sum) % 6) + 1;
                return $"var(--tile-{idx})";
            }
        }
    }
}
