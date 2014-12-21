using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScnSyntax {
    
    static class ScnSyntaxCompile {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            var keywords = Properties.Keywords.Default;
            var paths = new[] {
                @"(?<=include )%",
                @"(?<=timetable\:)%",
                @"(?<=trainset )%",
                @"(?<=node>P[3]sound>P[3])%",
                @"(?<=node>P[3]track>P[9])%",
                @"(?<=node>P[3]track>P[11])%"
            };
            var syntax = new PCR[] {
                new PCR("LineEnding", @"\r?\n"),
                new PCR("SingleLF", @"[^\r]\n"),
                new PCR("CRLF", @"\r\n"),
                new PCR("Tab", @"\t"),
                new PCR("HtmlClosingSlash", "</"),
                new PCR("HtmlEquals", "="),
                new PCR("HtmlStyleSheet", "(?<=<style>).*?(?=</style>)", RegexOptions.Singleline),
                new PCR("HWhiteSpace", @"[ \t;]+"),
                new PCR("VWhiteSpace", @"\r?\n"),
                new PCR("XVWhiteSpace", @"(?:\r\n){2,}"),
                new PCR("LineEnd", @"^ +| +(?=[\r\n])", RegexOptions.Multiline),
                new PCR("LineInterleave", @"(?:\r?\n){2}", RegexOptions.None),
                new PCR("Comment", @"//.*$", RegexOptions.Multiline),
                new PCR("CommentLine", @"^(\s*)//(.*)$", RegexOptions.Multiline),
                new PCR("NonCommentLine", @"^(\s*)(.*)$", RegexOptions.Multiline),
                new PCR("Keyword1", keywords.Set1.Cast<string>(), PcrOptions.Keywords, RegexOptions.IgnoreCase),
                new PCR("Keyword2", keywords.Set2.Cast<string>(), PcrOptions.Keywords, RegexOptions.IgnoreCase),
                new PCR("Keyword3", keywords.Set3.Cast<string>(), PcrOptions.Keywords, RegexOptions.IgnoreCase),
                new PCR("Keyword4", keywords.Set4.Cast<string>(), PcrOptions.Keywords, RegexOptions.IgnoreCase),
                new PCR("Keyword5", keywords.Set5.Cast<string>(), PcrOptions.Keywords, RegexOptions.IgnoreCase),
                new PCR("Number", @"(?<=[ ;\n]|^)[+-]?[\d.]+(?=[ ;\r\n])"),
                new PCR("Time", @"\b\d?\d:\d\d\b"),
                new PCR("Command", @"\$\w+"),
                new PCR("Special", @"\*{1}"),
                new PCR("Path", paths, PcrOptions.Special),
                new PCR("ExplicitTexExt", @"\.(?:bmp|tga|dds)(?=[\b\r\n ])", RegexOptions.IgnoreCase),
                new PCR("FirstActiveDynamic", "(?<=node>P[2])%(?= dynamic>P[4](?:headdriver|reardriver))", PcrOptions.Precompiled),
                new PCR("Include", "(?<=include )%(?= .*?end)", PcrOptions.Precompiled),
                new PCR("IncludeSimple", "(?<=include )%(?= end)", PcrOptions.Precompiled),
                new PCR("Timetable", @"(?<=trainset )%|(?<=Timetable\:)%", PcrOptions.Precompiled, RegexOptions.IgnoreCase),
                new PCR("CommandInclude", @"(?<=// *\$f +[a-z]{2} +)[^ ]+", RegexOptions.IgnoreCase),
                new PCR("TimetableFrame", @"[\[\]\|_]|(?<= {4,})[1-2]|(?<=\-)[1-2](?=\-)|(?<=_)[1-2](?=_)|\-{2,}"),
                new PCR("TimetableTime", @"(?<=[1-2] {1,3})\d?\d\.\d\d(?= {1,3}|)"),
                new PCR("NodeParams", @"(?<=node)(?: (%))+?(?= end[a-z]*)", PcrOptions.Precompiled),
                new PCR("IncludeParams", @"(?<=include)(?: (%))+?(?= end)", PcrOptions.Precompiled),
                new PCR("EventParams", @"(?<=event)(?: (%))+?(?= endevent)", PcrOptions.Precompiled),
                new PCR("TrainsetParams", @"(?<=trainset)(?: (%)){4}", PcrOptions.Precompiled)
            };
            Regex.CompileToAssembly(syntax, new AssemblyName("ScnSyntax"));
        }

        internal enum PcrOptions { Precompiled, Special, Keywords };

        class PCR : RegexCompilationInfo {

            const string NS = "ScnSyntax";
            
            private static Regex RxParams = new Regex(@">P\[(\d+)\]", RegexOptions.Compiled);

            public PCR(string name, string exp, RegexOptions options = RegexOptions.None)
                : base(exp, options, name, NS, true) { }

            public PCR(string name, string exp, PcrOptions options, RegexOptions regexOptions = RegexOptions.None)
                : base(PreCompile(exp), regexOptions, name, NS, true) { }

            public PCR(string name, IEnumerable<string> exp, PcrOptions options, RegexOptions regexOptions = RegexOptions.None)
                : base(Pack(exp, options), regexOptions, name, NS, true) { }

            private static string Pack(IEnumerable<string> aexp, PcrOptions options) {
                if (options.HasFlag(PcrOptions.Special))
                    return "(" + String.Join("|", aexp.Select(i => PreCompile(i))) + ")\\b";
                if (options.HasFlag(PcrOptions.Precompiled))
                    return "\\b(" + String.Join("|", aexp.Select(i => PreCompile(i))) + ")\\b";
                if (options.HasFlag(PcrOptions.Keywords))
                    return "\\b(" + String.Join("|", String.Join("|", aexp)) + ")\\b";
                return String.Join("|", aexp);
            }

            private static string PreCompile(string exp) {
                return Unpack(exp).Replace("(?<=", @"(?<=\b").Replace(" ", @"[ ;\r\n]+").Replace("%", @"[^ ;\r\n]+");
            }

            private static string Unpack(string pcExp) { return RxParams.Replace(pcExp, Unpacker); }

            private static string Unpacker(Match match) { return "(?: %){" + match.Groups[1].Value + "} "; }

        }

    }
}
