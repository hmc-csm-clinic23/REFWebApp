import { React, useState, useEffect, useContext } from "react";
import { Context } from "../../App";

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
    setMetricsList(
      data.map((stt) => ({
          ...stt,
          metrics: stt.metrics,
          transcriptions: stt.transcriptions,

      })),
        );
        console.log(metricsList.metrics);
        console.log(metricsList.transcriptions);

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
  */
  return (
    <div>
      
    </div>
  );
}

export default Evaluation;
