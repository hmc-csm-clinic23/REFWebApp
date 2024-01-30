import React from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import "./home.css";

function Home() {
  return (
    <div className="homePage">
      <div className="homeTitle">
        Evaluate Speech-to-Text Models at the Click of a Button{" "}
      </div>
      <div className="homeSubtitle">
        Run STTs on curated audio environments and assess accuracy, speed,
        memory, usability, and more.
      </div>
      <button className="tryButton">
        <NavLink tag={Link} to="/tool">
          Try It Out
        </NavLink>
      </button>
    </div>
  );
}

export default Home;
