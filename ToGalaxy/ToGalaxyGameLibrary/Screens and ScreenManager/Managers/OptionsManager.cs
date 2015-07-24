using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ToGalaxyCustomData;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class OptionsManager
    {
        public string OptionsAsset
        {
            get;
            private set;
        }

        public OptionsData OptionsData
        {
            get;
            private set;
        }

        public OptionsManager(string optionsAsset)
        {
            OptionsAsset = optionsAsset;
        }

        public void LoadContent(ContentManager content)
        {
            // Get the path of the save game
            DirectoryInfo contentDirectory = new DirectoryInfo(content.RootDirectory);
            string fullpath = contentDirectory.FullName + "/" + OptionsAsset;

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(OptionsData));
                XmlReader reader = XmlReader.Create(stream);

                // Use the Deserialize method to restore the object's state.
                try
                {
                    OptionsData = (OptionsData)serializer.Deserialize(reader);
                }
                catch
                {
                    OptionsData = content.Load<OptionsData>("XML/Settings/DefaultOptions");
                }
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }
    }
}
