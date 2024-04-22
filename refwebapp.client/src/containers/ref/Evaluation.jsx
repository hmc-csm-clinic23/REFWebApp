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

  }
  useEffect(() => {
    console.log("hi");
    postMetrics();
  }, []);

  return (
    <div>
      {loading
      ? <h1> Loading... This may take a while</h1>
      : metricsList.map(metricsList => (
        <EvalDisplay
        key={metricsList.sttName}
        stt={metricsList.sttName}
        refData={metricsList.refData}
        scenarios={selections.ScenarioList}
        />
      ))}
    </div>
  );
}

export default Evaluation;
