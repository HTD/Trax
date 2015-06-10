using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax {
    interface IColorScheme {
        Color Background { get; set; }
        Color Text { get; set; }
        Color Comment { get; set; }
        Color Identifier { get; set; }
        Color Keyword { get; set; }
        Color Keyword1 { get; set; }
        Color Keyword2 { get; set; }
        Color Keyword3 { get; set; }
        Color Keyword4 { get; set; }
        Color Number { get; set; }
        Color Time { get; set; }
        Color Special { get; set; }
        Color LineNumber { get; set; }
        Color ServiceLine { get; set; }
        Color Carret { get; set; }
        Color Command { get; set; }
        Color Path { get; set; }
        Color Selection { get; set; }
        Color SameWord { get; set; }
        Color SameWordText { get; set; }
        Color ProjectText { get; set; }
        Color ProjectSelection { get; set; }
        Color ProjectIcon { get; set; }
        Color SearchResultBack { get; set; }
        Color SearchResultFrame { get; set; }
        Color ReplaceResultBack { get; set; }
        Color ReplaceResultFrame { get; set; }
    }
}
