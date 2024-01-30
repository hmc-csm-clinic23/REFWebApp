import React, { Component } from "react";

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <div>
          <tr>
            <div>Date</div>
            <div>Temp. (C)</div>
            <div>Temp. (F)</div>
            <div>Summary</div>
          </tr>
        </div>
        <div>
          {forecasts.map((forecast) => (
            <tr key={forecast.date}>
              <div>{forecast.date}</div>
              <div>{forecast.temperatureC}</div>
              <div>{forecast.temperatureF}</div>
              <div>{forecast.summary}</div>
            </tr>
          ))}
        </div>
      </table>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      FetchData.renderForecastsTable(this.state.forecasts)
    );

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch("weatherforecast");
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
}
