﻿
using BriefFiniteElementNet.CSparse.Double;
using BriefFiniteElementNet.CSparse.Storage;

namespace BriefFiniteElementNet.Solver
{
    using System;

    /// <summary>
    /// Represents an interface for a solver to solve A*x = b.
    /// 
    /// </summary>
    public interface ISolver 
    {
        //Model Target { get; }

        /// <summary>
        /// Gets or sets A in A*x=b.
        /// </summary>
        /// <value>
        /// A value in A*x=b equation.
        /// </value>
        CompressedColumnStorage A { get; set; }

        /// <summary>
        /// Gets a value indicating whether the solver is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets the type of the solver.
        /// </summary>
        SolverType SolverType { get; }

        /// <summary>
        /// Initializes the solver regarding <see cref="A" /> matrix.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Solves a the system of A*x=b and store the x in <see cref="result" />.
        /// </summary>
        /// <param name="input">Right hand side vector.</param>
        /// <param name="result">Solution vector.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        /// Solver result.
        /// </returns>
        SolverResult Solve( double[] input, double[] result,out string message);
    }
}