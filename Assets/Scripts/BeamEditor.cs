using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Beam))]
public class BeamEditor : Editor {
	private void OnSceneGUI() {
		Beam beam = (Beam)target;

		//Draws the circle
		Handles.color = Color.blue;
		Handles.DrawWireArc(beam.transform.position, Vector3.forward, Vector3.up, 360, beam.beamRadius);

		//Draws the angle lines
		Vector3 beamAngleA = beam.DirectionFromAngle(-beam.beamAngle / 2f, false);
		Vector3 beamAngleB = beam.DirectionFromAngle(beam.beamAngle / 2f, false);
		Handles.DrawLine(beam.transform.position, beam.transform.position + beamAngleA * beam.beamRadius);
		Handles.DrawLine(beam.transform.position, beam.transform.position + beamAngleB * beam.beamRadius);

		//Draws a line to each target
		Handles.color = Color.red;
		foreach (Transform visibleTarget in beam.visibleTargets) {
			Handles.DrawLine(beam.transform.position, visibleTarget.position);
		}
	}
}
