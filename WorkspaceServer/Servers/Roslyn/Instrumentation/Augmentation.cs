﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MLS.Protocol.Instrumentation;

namespace WorkspaceServer.Servers.Roslyn.Instrumentation
{
    public class Augmentation
    {
        public Augmentation(
            CSharpSyntaxNode associatedStatment,
            IEnumerable<ISymbol> locals,
            IEnumerable<ISymbol> fields,
            IEnumerable<ISymbol> parameters,
            IEnumerable<ISymbol> internalLocals,
            FilePosition position = null)
        {
            AssociatedStatement = associatedStatment ?? throw new ArgumentNullException(nameof(associatedStatment));
            Locals = locals ?? Array.Empty<ISymbol>();
            Fields = fields ?? Array.Empty<ISymbol>();
            Parameters = parameters ?? Array.Empty<ISymbol>();
            InternalLocals = internalLocals ?? Array.Empty<ISymbol>();

            if (position == null)
            {
                var linePosition = AssociatedStatement.GetLocation().GetLineSpan();
                CurrentFilePosition = new FilePosition
                {
                    Line = linePosition.StartLinePosition.Line,
                    Character = linePosition.StartLinePosition.Character,
                    File = Path.GetFileName(linePosition.Path)
                };
            }
            else
            {
                CurrentFilePosition = position;
            }
        }

        public Augmentation withPosition(FilePosition position) => new Augmentation(
            AssociatedStatement,
            Locals,
            Fields,
            Parameters,
            InternalLocals,
            position);

        public CSharpSyntaxNode AssociatedStatement { get; }
        public IEnumerable<ISymbol> Locals { get; }
        public IEnumerable<ISymbol> Fields { get; }
        public IEnumerable<ISymbol> Parameters { get; }
        public IEnumerable<ISymbol> InternalLocals { get; }
        public FilePosition CurrentFilePosition { get; }
    }
}
