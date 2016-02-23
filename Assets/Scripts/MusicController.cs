using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
	AudioSource audioClip;
	float[] spectrum = new float[1024];
	private float[] samples = new float[1024];
	private float dbValue;
	private float rmsValue;
	private float pitchValue;
	private float fSample;

	void Start() {
		audioClip = GetComponent<AudioSource>();
		fSample = AudioSettings.outputSampleRate;
	}

	void GetVolume(){
		
		audioClip.GetOutputData(samples, 0); // fill array with samples
		int i;
		float sum = 0;
		for (i=0; i < 1024; i++){
			sum += samples[i]*samples[i]; // sum squared samples
		}
		rmsValue = Mathf.Sqrt(sum/1024); // rms = square root of average
		dbValue = 20* Mathf.Log10(rmsValue/0.01f); // calculate dB
		Debug.Log(dbValue);
		if (dbValue < -160) dbValue = -160; // clamp it to -160dB min


		audioClip.GetSpectrumData(spectrum, 0, FFTWindow.Hanning); //Gets the spectrogram which is then used to find highest amplitude frequency
		float maxV = 0;
		int maxN = 0;
		for (i=0; i < 1024; i++){ // find max 
			if (spectrum[i] > maxV && spectrum[i] > 0.02f){
				maxV = spectrum[i];
				maxN = i; // maxN is the index of max
			}
		}
		float freqN = maxN; // pass the index to a float variable
		if (maxN > 0 && maxN < 1024-1){ // interpolate index using neighbours
			float dL = spectrum[maxN-1]/spectrum[maxN];
			float dR = spectrum[maxN+1]/spectrum[maxN];
			freqN += 0.5f*(dR*dR - dL*dL);
		}
		pitchValue = freqN*(fSample/2)/1024; // convert index to frequency
	}

	void Update () {
		//audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		GetVolume();
		Vector3 currentScale = new Vector3(1,1,1);
		currentScale *= Mathf.Max(pitchValue / 1000f, 0.2f); 
		GetComponent<Transform>().localScale = currentScale;
	}
}
