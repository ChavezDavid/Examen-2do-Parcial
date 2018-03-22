﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efecto
{
    class DelayAtenuado : ISampleProvider
    {
        private ISampleProvider fuente;

        int offsetTiempoMS;

        public float factor;

        public int OffsetTiempoMS
        {
            get
            {
                return OffsetTiempoMS;
            }
            set
            {
                offsetTiempoMS = value;
            }
        }

        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                if (value > 1)
                    factor = 1;
                else if (value < 0)
                    factor = 0;
                factor = value;
            }
        }

        List<float> muestras = new List<float>();

        /*public DelayAtenuado(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 500;
            Factor = 0.5f;
            //50ms - 5000ms
        }*/

        public DelayAtenuado(ISampleProvider fuente, int offsetTiempoMS, float factor)
        {
            this.fuente = fuente;
            OffsetTiempoMS = offsetTiempoMS;
            Factor = factor;
            if (offsetTiempoMS > 5000)
                OffsetTiempoMS = 5000;
            else if (offsetTiempoMS < 50)
                OffsetTiempoMS = 50;
            if (factor > 1)
                Factor = 1;
            else if (factor < 0)
                Factor = 0;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        //Offset es el numero de muestras leidas
        public int Read(float[] buffer, int offset, int count)
        {
            //Calculo de tiempo
            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido = (float)muestras.Count / (float)fuente.WaveFormat.SampleRate;
            int muestrasTranscurridas = muestras.Count;
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestrasOffsetTiempo = (int)(((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            //Añadir muestras
            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);
            }

            //Modificar muestras
            if (tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[i] += ((muestras[muestrasTranscurridas + i - numMuestrasOffsetTiempo]) * Factor);
                }
            }

            return read;
        }
    }
}
