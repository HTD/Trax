using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnEdit {
    interface IStyleScheme {
        FontStyle Identifier { get; set; }
        FontStyle Comment { get; set; }
        FontStyle Keyword { get; set; }
        FontStyle Keyword1 { get; set; }
        FontStyle Keyword2 { get; set; }
        FontStyle Keyword3 { get; set; }
        FontStyle Keyword4 { get; set; }
        FontStyle Path { get; set; }
        FontStyle Number { get; set; }
        FontStyle Time { get; set; }
        FontStyle Command { get; set; }
        FontStyle Special { get; set; }
        FontStyle SameWord { get; set; }
    }
}
