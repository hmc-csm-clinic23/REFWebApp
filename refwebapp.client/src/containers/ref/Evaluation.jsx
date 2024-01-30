import { React, useContext } from "react";
import { Context } from "../../App";

function Evaluation() {
  const { selections } = useContext(Context);
  return (
    <div>
      <h1>{selections.stts.map((stt) => stt.name)}</h1>
      <h1>
        {selections.scenarios.map((scenario) => [
          scenario.name,
          scenario.weight,
        ])}
      </h1>
      <h1>{selections.useStore.toString()}</h1>
      <h1>{selections.updateStore.toString()}</h1>
    </div>
  );
}

export default Evaluation;
