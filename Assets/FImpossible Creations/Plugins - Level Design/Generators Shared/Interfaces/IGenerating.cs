using UnityEngine;

namespace FIMSpace.Generating
{
    public interface IGenerating
    {
        //GameObject GetObject { get; }
        void Generate();
        /// <summary> Optional preview, can be empty </summary>
        void PreviewGenerate();
    }

}