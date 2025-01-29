using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WavUtils : MonoBehaviour
{
    public const int headerSize = 44;

    public static byte[] Convert(AudioClip clip)
    {
        try
        {
            var memoryStream = new MemoryStream();
            WriteHeader(memoryStream, clip);
            ConvertAndWrite(memoryStream, clip);

            var wavData = memoryStream.ToArray();

            return wavData;
        }
        catch (Exception exception)
        {
            Debug.LogError("Erro convertendo: " + exception);
            return null;
        }
    }

    public static AudioClip Convert(float[] data, int channels, int sampleRate)
    {
        var audioClip = AudioClip.Create("temp", data.Length, channels, sampleRate, false);
        audioClip.SetData(data, 0);
        return audioClip;
    }

    public static bool SaveToFile(string filePath, AudioClip audioClip)
    {
        if (!filePath.ToLower().EndsWith(".wav"))
            filePath += ".wav";

        var data = Convert(audioClip);
        return FileUtils.SaveToFile(filePath, data);
    }



    public static void WriteHeader(MemoryStream stream, AudioClip clip)
    {
        var channels = clip.channels;

        stream.Seek(0, SeekOrigin.Begin);

        var riff = Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        var chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        var wave = Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        var fmt = Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        var subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        var audioFormat = BitConverter.GetBytes((short)1);
        stream.Write(audioFormat, 0, 2);

        var numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        var sampleRate = BitConverter.GetBytes(clip.frequency);
        stream.Write(sampleRate, 0, 4);

        var byteRate = BitConverter.GetBytes(clip.frequency * channels * 2);
        stream.Write(byteRate, 0, 4);

        var blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        var bitsPerSample = BitConverter.GetBytes((short)16);
        stream.Write(bitsPerSample, 0, 2);

        var datastring = Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        var subChunk2 = BitConverter.GetBytes(clip.samples * channels * 2);
        stream.Write(subChunk2, 0, 4);
    }

    static void ConvertAndWrite(MemoryStream fileStream, AudioClip clip)
    {
        var samples = new float[clip.samples];

        clip.GetData(samples, 0);
        fileStream.Seek(headerSize, SeekOrigin.Begin);

        ///Tem que ser o dobro porque na realidade guarda em int16, que é o dobro de byte
        var bytesData = new byte[samples.Length * 2];

        ///De float para int16
        float rescaleFactor = 32767;

        for (var i = 0; i < samples.Length; i++)
        {
            var reescaled = BitConverter.GetBytes((short)(samples[i] * rescaleFactor));
            reescaled.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }


    public static AudioClip FromPcmBytes(byte[] bytes, string clipName = "pcm")
    {
        if (string.IsNullOrEmpty(clipName))
            return null;

        var pcmData = PcmData.FromBytes(bytes);
        var audioClip = AudioClip.Create(clipName, pcmData.Length, pcmData.Channels, pcmData.SampleRate, false);
        audioClip.SetData(pcmData.Value, 0);
        return audioClip;
    }


    public static AudioClip TrimSilence(AudioClip clip, float min)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency, false);
    }

    public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool stream)
    {
        int i;
        var samplesAbsolute = new List<float>(samples.Count);
        int startIndex = 0;
        for (i = 0; i < samples.Count; i++)
        {
            samplesAbsolute.Add(Mathf.Abs(samples[i]));
            if (i == 0 && samplesAbsolute[i] > min)
            {
                startIndex = i;
            }
        }

        samples.RemoveRange(0, startIndex);
        samplesAbsolute.RemoveRange(0, startIndex);

        const float bufferLength = 0.1f;
        var sampleSize = Mathf.CeilToInt(bufferLength * hz);
        var stepSize = Mathf.Max(1, sampleSize / 2);


        for (i = samplesAbsolute.Count - 1; i > 0; i -= stepSize)
        {
            if (Average(samplesAbsolute.GetRange(i - sampleSize, sampleSize)) > min)
            {
                break;
            }
        }

        float bufferDuration = 0.5f;
        var buffer = Mathf.FloorToInt(hz * bufferDuration);
        if (samples.Count - i > buffer)
            i += buffer;

        samples.RemoveRange(i, samples.Count - i);
        Debug.Log($"Duração: {((float)samples.Count) / hz}");

        var clip = AudioClip.Create("Clip", samples.Count, channels, hz, stream);

        clip.SetData(samples.ToArray(), 0);

        return clip;
    }

    private static float Average(List<float> data)
    {
        float sum = 0;
        for (int i = 0; i < data.Count; i++)
            sum += data[i];
        return sum / data.Count;
    }
}
