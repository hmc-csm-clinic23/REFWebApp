import { React, useState, useEffect } from "react";
import { AiOutlineCheck } from "react-icons/ai";
import { S3Client, GetObjectCommand } from "@aws-sdk/client-s3";
import { getSignedUrl } from "@aws-sdk/s3-request-presigner";

function AddScenarioCheckbox({ toggle, setToggle, name, groundTruth, audioFile,  }) {
  const [data, setData] = useState(null);

  useEffect(() => {
    const getObjectFromS3 = async (fileKey) => {
      try {
        const client = new S3Client({
          region: 'us-west-1',
          credentials: {
            accessKeyId: 'AKIA3XQL3GCMUFJ3ILUV',
            secretAccessKey: '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'
          }
        });
        const command = new GetObjectCommand({
          Bucket: 'nbl-audio-files',
          Key: fileKey.trim()
        });
        const url = await getSignedUrl(client, command, { expiresIn: 15*60 });
        //const response = await client.send(command);
        setData(url);
      } catch (error) {
        console.error('Error:', error);
      }
    };
      getObjectFromS3(audioFile);

  }, []);
  
  return toggle ? (
    <li className="addScenarioItem" onClick={setToggle}>
      <div className="leftSide">
        <span className="checkbox">
          <AiOutlineCheck />
        </span>
      </div>
      <div className="addScenarioContainer">
        <span className="itemText">{name}</span>
        <span className="itemText">{groundTruth}</span>
        <audio controls src={data} className=""></audio>
      </div>
    </li>
  ) : (
    <li className="addScenarioItem" onClick={setToggle}>
      <div className="leftSide">
        <span className="checkbox" />
      </div>
      <div className="addScenarioContainer">
        <span className="itemText">{name}</span>
        <span className="itemText">{groundTruth}</span>
        <audio controls className=""></audio>
      </div>
    </li>
  );
}

export default AddScenarioCheckbox;
