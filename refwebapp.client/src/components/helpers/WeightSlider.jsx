function WeightSlider({ toggle, weight, setWeight, i, scenarioSum }) {
  if (!toggle) return null;

  return (
    <div>
      <input
        className={weight > 50 ? "heigh" : "less"}
        type="range"
        min="1"
        max="100"
        step="1"
        value={weight}
        onChange={(e) => setWeight(i, e.target.value)}
      />
      <h1>{weight}</h1>
      <p>{((100 * weight) / scenarioSum).toFixed(2)}%</p>
    </div>
  );
}

export default WeightSlider;
