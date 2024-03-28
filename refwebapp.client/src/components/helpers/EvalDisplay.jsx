import { Evals } from "../.././components";
function EvalDisplay({ stt, metrics, transcriptions }) {
  const metricsList = metrics.reduceRight((all, item) => ({[item]: all}), {});
  const transcriptionsList = transcriptions.reduceRight((all, item) => ({[item]: all}), {});
  console.log(metrics);
  console.log(transcriptions);
  console.log(metrics.map((metrics) => metrics));
  return (
    <div>
      <h1>{stt}</h1>
      <div>
        {metrics.map((metrics, index) => 
        <Evals
        metrics={metrics}
        transcriptions={transcriptions[index]}
        />
        )}
      </div>
    </div>
  );
}

export default EvalDisplay;
