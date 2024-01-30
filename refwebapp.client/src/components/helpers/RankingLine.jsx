function RankingLine({ stt, score, accuracy, speed, memory, usability, api }) {
  return (
    <div className="rankingLine">
      <div className="rankingField">{stt}</div>
      <div className="rankingField">{score}</div>
      <div className="rankingField">{accuracy}</div>
      <div className="rankingField">{speed}</div>
      <div className="rankingField">{memory}</div>
      <div className="rankingField">{usability}</div>
      <div className="rankingField">{api}</div>
    </div>
  );
}

export default RankingLine;
