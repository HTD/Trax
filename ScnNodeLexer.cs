using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trax {

    /// <summary>
    /// SCN highly optimized finite state machine lexer (no regular expressions used here!)
    /// </summary>
    public class ScnNodeLexer {

        private static int TypeIndexLimit = 0x10000; // specified node type must occur in first 1MB
        private static Encoding Encoding = Encoding.GetEncoding(1250); // default single byte encoding
        private static IFormatProvider FP = System.Globalization.CultureInfo.InvariantCulture; // needed to have C default decimal separator
        private static System.Globalization.NumberStyles NS = System.Globalization.NumberStyles.Float; // needed for default float numbers format
        private static StringComparison CI = StringComparison.InvariantCultureIgnoreCase; // case ignoring comparison type

        /// <summary>
        /// Node object collection
        /// </summary>
        public Node[] Nodes;

        /// <summary>
        /// Creates lexer module and performs lexical analysis on specified source code
        /// </summary>
        /// <param name="source"></param>
        public ScnNodeLexer(string source, string type = null) {
            // type filter:
            if (type != null && source.IndexOf(' ' + type + ' ', 0, (new int[] { TypeIndexLimit, source.Length }).Min()) < 0) return;
            // initialization:
            int sourceLength = source.Length;
            int sourceLastIndex = sourceLength - 1;
            var values = new List<object>();
            States state = States.Scan;
            Stack<States> states = new Stack<States>();
            string fragment = "";
            char c;
            bool isEndLine = false;
            bool isWhiteSpace;
            bool isCommentStart, lastCommentStart = false, isComment = false;
            bool isEnd = false;
            Node node = null;
            List<Node> nodes = new List<Node>();
            // analysis:
            for (int i = 0; i < sourceLength; i++) {
                c = source[i];
                isEndLine = c == '\n';
                isWhiteSpace = c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == ';'; // all whitespace and separators defined in SCN format
                isCommentStart = c == '/';
                isComment = isCommentStart && lastCommentStart; // true on second '/' in a row
                lastCommentStart = isCommentStart;
                isEnd = isWhiteSpace || isComment || i == sourceLastIndex; // identifier is considered ended after whitespace character, comment start or end of data
                switch (state) {
                    case States.Scan:
                        if (isWhiteSpace) {
                            if (fragment.Equals("node", CI)) {
                                node = new Node() { SourceIndex = i - 4 };
                                state = States.MaxDistance;
                            }
                            fragment = "";
                        } else fragment += c;
                        break;
                    case States.Comment:
                        if (isEndLine) { state = states.Pop(); fragment = ""; }
                        break;
                    case States.MaxDistance:
                        if (c == '-' || c == '.' || (c >= '0' && c <= '9')) { fragment += c; continue; } else if (isEnd) {
                            node.MaxDistance = int.Parse(fragment);
                            state = States.MinDistance;
                            fragment = "";
                        } else throw new InvalidOperationException("Node MaxDistance must be a number");
                        break;
                    case States.MinDistance:
                        if (c == '-' || c == '.' || (c >= '0' && c <= '9')) { fragment += c; continue; } else if (isEnd) {
                            node.MinDistance = int.Parse(fragment);
                            state = States.Name;
                            fragment = "";
                        } else throw new InvalidOperationException("Node MinDistance must be a number");
                        break;
                    case States.Name:
                        if (!isWhiteSpace) fragment += c;
                        else if (isEnd) {
                            node.Name = fragment.TrimEnd(new[] { '/' });
                            if (node.Name.Equals("none", CI)) node.Name = null;
                            state = States.Type;
                            fragment = "";
                        }
                        break;
                    case States.Type:
                        if (!isWhiteSpace) fragment += c;
                        if (isEnd) {
                            node.Type = fragment.TrimEnd(new[] { '/' });
                            if (type != null && !node.Type.Equals(type, CI)) {
                                node = null;
                                state = States.Scan;
                            } else state = States.Value;
                            fragment = "";
                        }
                        break;
                    case States.Value:
                        if (!isWhiteSpace) fragment += c;
                        if (isEnd) {
                            fragment = fragment.TrimEnd(new[] { '/' });
                            if (fragment.Length > 0) {
                                if (fragment.Equals("end", CI) || fragment.StartsWith("end" + node.Type.Substring(0, 3), CI)) { // node ending
                                    node.SourceLength = i - node.SourceIndex;
                                    node.Values = values.ToArray();
                                    nodes.Add(node);
                                    node = null;
                                    values.Clear();
                                    state = States.Scan;
                                } else if (fragment.Equals("none", CI)) { // node null value
                                    values.Add(null);
                                } else { // node regular value (text or number)
                                    double n;
                                    bool isNumeric = Double.TryParse(fragment, NS, FP, out n);
                                    if (isNumeric) values.Add(n);
                                    else values.Add(fragment);
                                }
                                fragment = "";
                            }
                        }
                        break;
                }
                if (isComment && state != States.Comment) { states.Push(state); state = States.Comment; }
            }
            // fields:
            if (nodes.Count > 0) Nodes = nodes.ToArray();

        }

        /// <summary>
        /// SCN Node object
        /// </summary>
        public class Node {

            public int MaxDistance;
            public int MinDistance;
            public string Name;
            public string Type;
            public int SourceIndex;
            public int SourceLength;
            public object[] Values;

        }

        /// <summary>
        /// Possible lexer states
        /// </summary>
        enum States { Scan, Comment, MaxDistance, MinDistance, Name, Type, Value }

    }

}