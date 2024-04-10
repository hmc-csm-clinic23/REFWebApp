function RankingLine({ stt, score, accuracy, speed, wer, mer, wil, sim, dist }) {
  return (
    <div className="rankingLine">
      <div className="rankingField">{stt}</div>
      <div className="rankingField">{score.toFixed(2)}</div>
      <div className="rankingField">{accuracy.toFixed(2)}</div>
      <div className="rankingField">{speed}</div>
      <div className="rankingField">{wer.toFixed(2)}</div>
      <div className="rankingField">{mer.toFixed(2)}</div>
      <div className="rankingField">{wil.toFixed(2)}</div>
      <div className="rankingField">{sim.toFixed(2)}</div>
      <div className="rankingField">{dist.toFixed(2)}</div>
    </div>
  );
}

export default RankingLine;
