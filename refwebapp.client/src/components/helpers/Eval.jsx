import { React, useState, useEffect } from "react";
import { S3Client, GetObjectCommand } from "@aws-sdk/client-s3";
import { getSignedUrl } from "@aws-sdk/s3-request-presigner";
function Eval({ wer, mer, wil, sim, dist, transcriptions, groundTruths, audioFile }) {
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
      <div className="evalItem" onClick={() => setToggle(!toggle)}>
        <div className="evalText">[{`WER: ${wer.toFixed(2)}, MER: ${mer.toFixed(2)}, WIL: ${wil.toFixed(2)}, Similarity: ${sim.toFixed(2)}, Lev-Distance: ${dist.toFixed(2)}`}]</div>
        <div className="evalText">{transcriptions}</div>
        <div className="evalText">{groundTruths}</div>
        {toggle ? <audio controls src={data} className=""></audio> : <audio controls className=""></audio>}
      </div>
  );
}

export default Eval;
