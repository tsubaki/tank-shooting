using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class TestData_EE_Stage_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/ExcelData/TestData.xls";
    private static readonly string[] sheetNames = { "Stage1", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read))
            {
                var book = new HSSFWorkbook(stream);

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/ExcelData/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (EE_Stage)AssetDatabase.LoadAssetAtPath(exportPath, typeof(EE_Stage));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<EE_Stage>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new EE_Stage.Param();
			
					cell = row.GetCell(0); p.Time = (float)(cell == null ? 0 : cell.NumericCellValue);
					p.pos = new int[5];
					cell = row.GetCell(1); p.pos[0] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.pos[1] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.pos[2] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.pos[3] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.pos[4] = (int)(cell == null ? 0 : cell.NumericCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
