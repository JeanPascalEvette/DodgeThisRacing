using UnityEditor;

[CustomEditor(typeof(CarController))]
public class CarControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CarController myTarget = (CarController)target;
        EditorGUILayout.IntField("Calculated Speed", (int)myTarget.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude);
        EditorGUILayout.IntField("Gear", myTarget.currentGear);
        EditorGUILayout.FloatField("Gear Value", myTarget.GearValue());
        EditorGUILayout.FloatField("RPM", myTarget.rpm);
        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical();
        DrawDefaultInspector();
        EditorGUILayout.EndVertical();

    }
}