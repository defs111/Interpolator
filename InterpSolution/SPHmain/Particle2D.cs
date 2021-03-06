﻿using SimpleIntegrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

namespace SPH_2D {
    public interface IParticle2D : IScnObj {
        /// <summary>
        /// Х координата частицы
        /// </summary>
        double X { get; }

        /// <summary>
        /// Y координата частицы
        /// </summary>
        double Y { get; }

        /// <summary>
        /// Скорость по X
        /// </summary>
        double dX { get; }

        /// <summary>
        /// Скорость по Y
        /// </summary>
        double dY { get; }

        /// <summary>
        /// Скорость
        /// </summary>
        IPosition2D Vel { get; }

        /// <summary>
        /// Соседи частицы
        /// </summary>
        List<IParticle2D> Neibs { get; }

        /// <summary>
        /// получить максимальный радиус сглаживания
        /// </summary>
        /// <returns></returns>
        double GetHmax();

        /// <summary>
        /// Расстояние до частицы
        /// </summary>
        /// <param name="particle">до этой частицы</param>
        /// <returns></returns>
        double GetDistTo(IParticle2D particle);

        /// <summary>
        /// Сколько зависимых операций необходимо частице на каждом шаге интегрирования
        /// </summary>
        int StuffCount { get; }

        /// <summary>
        /// Произвести зависимую операцию под индексом stuffIndex
        /// </summary>
        /// <param name="stuffIndex"></param>
        void DoStuff(int stuffIndex);


    }

    /// <summary>
    /// Абстрактный класс представляющий частицу для SPH 2D
    /// </summary>
    public abstract class Particle2DBase: Position2D, IParticle2D {
        #region IParticle 2D impl
        public IPosition2D Vel { get; private set; }
        public double dX {
            get {
                return Vel.X;
            }

            set {
                Vel.X = value;
            }
        }

        public double dY {
            get {
                return Vel.Y;
            }

            set {
                Vel.Y = value;
            }
        }

        public List<IParticle2D> Neibs { get; private set; }
        public double GetDistTo(IParticle2D particle) {
            double deltX = X - particle.X;
            double deltY = Y - particle.Y;
            return Sqrt(deltX * deltX + deltY * deltY);
        }
        public double GetHmax() {
            return hmax;
        }
        #endregion

        public double hmax;
        public Particle2DBase(double hmax) {
            this.hmax = hmax;
            Name = "Particle";

            Vel = new Position2D();
            Vel.Name = "Vel";
            AddChild(Vel);

            AddDiffVect(Vel);
            Neibs = new List<IParticle2D>(30);

            
        }

        #region Abstract
        public abstract int StuffCount { get; }

        public abstract void DoStuff(int stuffIndex);



        #endregion

        #region Static
        /// <summ
        public static double dW_func(double r_shtr,double h) {
            double q = Math.Abs(r_shtr) / h;
            if(q > 2.0)
                return 0.0;

            double a = -2.0 / (3.0 * h * h);
            double result = 0;

            if(q < 0.66666)
                result = 1;
            else if(q >= 0.66666 && q < 1.0)
                result = 3.0 * q * (4.0 - 3.0 * q) / 4.0;
            else if(q >= 1.0 && q <= 2.0)
                result = 3.0 * (2.0 - q) * (2.0 - q) / 4.0;

            return result * a;
        }
        public static double W_func(double r_shtr,double h) {
            double q = Math.Abs(r_shtr) / h;
            if(q > 2.0)
                return 0.0;
            double a = 2.0 / (3.0 * h);
            double result = 0;

            if(q >= 0 && q <= 1.0)
                result = 0.25 * (4d - 6 * q * q + 3 * q * q * q);
            else if(q > 1.0 && q <= 2.0)
                result = 0.25 * (2.0 - q) * (2.0 - q) * (2.0 - q);

            return result * a;
        }
        #endregion

    }

}
