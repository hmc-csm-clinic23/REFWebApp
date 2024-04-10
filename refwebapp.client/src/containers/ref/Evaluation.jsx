import { React, useState, useEffect, useContext } from "react";
import { Context } from "../../App";
import { EvalDisplay } from "../.././components";
import "./evaluation.css";

function Evaluation() {
  const { selections } = useContext(Context);
  const [metricsList, setMetricsList] = useState([]);
  const [loading, setLoading] = useState(true);
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
          ScenarioList: selections.ScenarioList,
          WeightList: selections.WeightList,
        })
      });
    setLoading(true)
    const data = await response.json();
    setMetricsList(data);
    setLoading(false);
    console.log(metricsList.map(metricsList => metricsList.sttName));
    console.log(metricsList.map(metricsList => metricsList.sttName.toString()));
    //console.log(metricsList.map(metricsList => metricsList.refData.metrics.toString()));
    //console.log(metricsList.map(metricsList => metricsList.refData.transcriptions.toString()));

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
      {loading
      ? <h1> Loading... This may take a while</h1>
      : metricsList.map(metricsList => (
        <EvalDisplay
        stt={metricsList.sttName}
        refData={metricsList.refData}
        scenarios={selections.ScenarioList}
        />
      ))}
    </div>
  );
}

export default Evaluation;
