using System;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( TestControl ) )]
public class TestControlInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestControl ctrl = target as TestControl;

        if( GUILayout.Button( "Spawn or Reset Prefab" ) )
        {
            DeleteChildren( ctrl );

            if( null != ctrl.testPrefab )
            {
                ctrl.spawnedObject = Instantiate( ctrl.testPrefab, ctrl.transform, false ) as GameObject;
            }
        }

        if( null != ctrl.spawnedObject )
        {
            GUILayout.Label( "Not functioning as expected" );

            if( GUILayout.Button( "Method 1: Quaternion.Euler" ) )
            {
                ctrl.spawnedObject.transform.localRotation *= Quaternion.Euler( 0f, 0f, 90f );
            }

            if( GUILayout.Button( "Method 2: transform.Rotate" ) )
            {
                ctrl.spawnedObject.transform.Rotate( new Vector3( 0f, 0f, 90f ), Space.Self );
            }

            if( GUILayout.Button( "Method 3: Quaternion.AngleAxis" ) )
            {
                ctrl.spawnedObject.transform.localRotation *= Quaternion.AngleAxis( 90f, Vector3.forward );
            }

            if( GUILayout.Button( "Method 4: transform.forward to Quaternion.AngleAxis" ) )
            {
                Vector3 localForwardVec = ctrl.transform.worldToLocalMatrix.MultiplyVector( ctrl.spawnedObject.transform.forward );
                ctrl.spawnedObject.transform.localRotation *= Quaternion.AngleAxis( 90f, -localForwardVec );
            }

            if( GUILayout.Button( "Method 5: RotateAoundLocal - deprecated" ) )
            {
                Vector3 localForwardVec = ctrl.transform.worldToLocalMatrix.MultiplyVector( ctrl.spawnedObject.transform.forward );
                ctrl.spawnedObject.transform.RotateAroundLocal( Vector3.back, 90f );
            }

            GUILayout.Label( "Works as expected" );

            if( GUILayout.Button( "Method 6: RotateAound" ) )
            {
                Vector3 worldVec = ctrl.transform.localToWorldMatrix.MultiplyVector( Vector3.back );
                ctrl.spawnedObject.transform.RotateAround( ctrl.spawnedObject.transform.position, worldVec, 90f );
            }
        }
    }

    private void DeleteChildren( TestControl ctrl )
    {
        int childCount = ctrl.transform.childCount;

        for( int iChild = childCount - 1; 0 <= iChild; --iChild )
        {
            DestroyImmediate( ctrl.transform.GetChild( iChild ).gameObject );
        }
    }
}
