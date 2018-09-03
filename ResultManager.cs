using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects")]
public class ResultManager : ScriptableObject {

	public List<float> distance;
    public List<float> velocity;
	public List<Vector3> speed;
    public List<Vector3> positions;
    public List<Quaternion> rotations;
    public float toHigh = 0.0f;
    public float toLow = 0.0f;
    public float justRightComplementaryAngle = 0.0f;
    public float toLeft = 0.0f;
    public float toRight = 0.0f;
    public float justRightGuidingAngle = 0.0f;


    public void Clear() {
		distance.Clear ();
        velocity.Clear();
		speed.Clear ();
		positions.Clear ();
		rotations.Clear ();
        toHigh = 0.0f;
        toLow = 0.0f;
        justRightGuidingAngle = 0.0f;
        toLeft = 0.0f;
        toRight = 0.0f;
        justRightComplementaryAngle = 0.0f;
    }
}
