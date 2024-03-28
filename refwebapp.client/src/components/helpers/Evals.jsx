function Evals({ metrics, transcriptions }) {
  const metricLabels = ["WER: ", "MER: ", "WIL: ", "SIM: ", "L-DIST: "];
  return (
    <div>
      {metrics.map((metrics, index) => 
      <>
        <div>[{metrics.map((metric, i) => [metricLabels[i], metric, i < 4 && ", "])}]</div>
        <div>{transcriptions[index]}</div>
      </>
      )}
    </div>
  );
}

export default Evals;
