import { React, useState, useEffect, useContext } from "react";
import { Context } from "../../App";
import { EvalDisplay } from "../.././components";
import "../ranking/ranking.css";

function Evaluation() {
  const { selections } = useContext(Context);
  const [metricsList, setMetricsList] = useState([]);
  async function postMetrics() {
    console.log("hi");
    console.log("hi");
    const response = await fetch('metrics',
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          SttList: selections.SttList,
          ScenarioList: selections.ScenarioList
        })
      });
    const data = await response.json();
    setMetricsList(data);
    console.log(metricsList.map(metricsList => metricsList.sttName));
    console.log(metricsList.map(metricsList => metricsList.sttName.toString()));
    console.log(metricsList.map(metricsList => metricsList.refData.metrics.toString()));
    console.log(metricsList.map(metricsList => metricsList.refData.transcriptions.toString()));

  }
  useEffect(() => {
    console.log("hi");
    postMetrics();
  }, []);
  /*
  <h1>{selections.stts.map((stt) => stt.name)}</h1>
      <h1>
        {selections.scenarios.map((scenario) => [
          scenario.name,
          scenario.weight,
        ])}
      </h1>
      <h1>
        {selections.scenarios.map((scenario) => [
          scenario.audios.map((audio) => [audio.id]),
        ])}
      </h1>
      <h1>{selections.useStore.toString()}</h1>
      <h1>{selections.updateStore.toString()}</h1>
      <h1>{metricsList.map(metricsList => metricsList.sttName)}</h1>
      <h1>{metricsList.map(metricsList => metricsList.refData.metrics)}</h1>
      <h1>{metricsList.map(metricsList => metricsList.refData.transcriptions)}</h1>
  */
  return (
    <div>
      {metricsList.map(metricsList => (
        <EvalDisplay
        stt={metricsList.sttName}
        metrics={metricsList.refData.metrics}
        transcriptions={metricsList.refData.transcriptions}
        />
      ))}
    </div>
  );
}

export default Evaluation;
