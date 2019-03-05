using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Beam : MonoBehaviour {

	public float beamStrength;
	public ForceMode2D beamForceMode;
	public float beamRadius;
	public float beamAngle;
	public float meshResolution;
	private MeshFilter beamMeshFilter;
	private MeshRenderer beamMeshRenderer;
	public int edgeResolveIterations;
	public float edgeDistanceThreshold;
	private Mesh beamMesh;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
	[HideInInspector] public List<Transform> visibleTargets= new List<Transform>();

	// Start is called before the first frame update
	void Start() {
		beamMeshFilter = GetComponent<MeshFilter>();
		beamMeshRenderer = GetComponent<MeshRenderer>();
		beamMesh = new Mesh();
		beamMeshFilter.mesh = beamMesh;
	}

	private void Update() {
		
	}

	// Update is called once per frame
	void LateUpdate() {
		FindTargets();
		DrawBeam();
		PushPull();
    }

	private void PushPull() {
		foreach (Transform target in visibleTargets) {
			Vector3 directionToTarget = (target.position - transform.position).normalized;
			target.GetComponent<Rigidbody2D>().AddForce(directionToTarget * beamStrength * Time.deltaTime, beamForceMode);
		}
	}

	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees -= transform.eulerAngles.z;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
	}

	public void FindTargets() {
		visibleTargets.Clear();
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, beamRadius, targetMask);
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius[i].transform;
			Vector3 directionToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.up, directionToTarget) < beamAngle / 2f) {
				float distanceToTarget = Vector3.Distance(transform.position, target.position);
				if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)) {
					visibleTargets.Add(target);
				}
			}
		}
	}

	private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;
		for (int i = 0; i < edgeResolveIterations; i++) {
			float angle = (minAngle + maxAngle) / 2f;
			ViewCastInfo newViewCast = ViewCast(angle);
			bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
			if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded) {
				minAngle = angle;
				minPoint = newViewCast.point;
			} else {
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}
		return new EdgeInfo(minPoint, maxPoint);
	}

	public void DrawBeam() {
		int stepCount = Mathf.RoundToInt(beamAngle * meshResolution);
		float stepAngleSize = beamAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= stepCount; i++) {
			float angle = (-transform.eulerAngles.z) - beamAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);
			if (i > 0) {
				bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)) {
					EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
					if (edge.pointA != Vector3.zero) {
						viewPoints.Add(edge.pointA);
					}
					if (edge.pointB != Vector3.zero) {
						viewPoints.Add(edge.pointB);
					}
				}
			}
			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;

		}
		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];
		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) {
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
			if (i < vertexCount - 2) {
				triangles[i * 3 + 0] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		beamMesh.Clear();
		beamMesh.vertices = vertices;
		beamMesh.triangles = triangles;
		beamMesh.RecalculateNormals();
	}

	private ViewCastInfo ViewCast(float globalAngle) {
		Vector3 direction = DirectionFromAngle(globalAngle, true);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, beamRadius, obstacleMask);
		if (hit) {
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		} else {
			return new ViewCastInfo(false, transform.position + direction * beamRadius, beamRadius, globalAngle);
		}
	}


	public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float distance;
		public float angle;

		public ViewCastInfo(bool hit, Vector3 point, float distance, float angle) {
			this.hit = hit;
			this.point = point;
			this.distance = distance;
			this.angle = angle;
		}
	}

	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 pointA, Vector3 pointB) {
			this.pointA = pointA;
			this.pointB = pointB;
		}
	}
}
