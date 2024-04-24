import { React, useState, useEffect } from "react";
import { S3Client, GetObjectCommand } from "@aws-sdk/client-s3";
import { getSignedUrl } from "@aws-sdk/s3-request-presigner";

function AudioList({ groundTruth, transcription, wer, mer, wil, sim, dist, audioFile }) {
  const [toggle, setToggle] = useState(false);
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

  return (
    <li className="audioList" onClick={() => setToggle(!toggle)}>
      <div className="evalsText">{groundTruth}</div>
      <div className="evalsText">{transcription}</div>
      <div className="evalsText">{wer.toFixed(2)}</div>
      <div className="evalsText">{mer.toFixed(2)}</div>
      <div className="evalsText">{wil.toFixed(2)}</div>
      <div className="evalsText">{sim.toFixed(2)}</div>
      <div className="evalsText">{dist.toFixed(2)}</div>
      {toggle ? <audio controls src={data} className="audioPlayer"></audio> : <audio controls className="audioPlayer"></audio>}
    </li>
  );
}

export default AudioList;
