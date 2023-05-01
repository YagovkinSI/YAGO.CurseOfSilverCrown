import * as React from 'react';
import { Link, useLocation } from 'react-router-dom';
import * as WeatherForecastsStore from '../../store/WeatherForecasts';
import { useAppDispatch, useAppSelector } from '../../store';
import { weatherForecastsActionCreators } from '../../store/WeatherForecasts';

const FetchData :  React.FC = () => {
  const state = useAppSelector(state => state.weatherForecastsReducer);
  const dispatch = useAppDispatch();

  const pathName = useLocation().pathname;
  let startDateIndexString = pathName.replace('/fetch-data', '').replace('/', '');
  const startDateIndex = parseInt(startDateIndexString, 10) || 0;  
  weatherForecastsActionCreators.requestWeatherForecasts(dispatch, startDateIndex);

  const renderForecastsTable = () => {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {state != undefined
            ? state.forecasts.map((forecast: WeatherForecastsStore.WeatherForecast) =>
              <tr key={forecast.date}>
                <td>{forecast.date}</td>
                <td>{forecast.temperatureC}</td>
                <td>{forecast.temperatureF}</td>
                <td>{forecast.summary}</td>
              </tr>
            )
            : <></>
          }
        </tbody>
      </table>
    );
  }

  const renderPagination = () => {
    const startDateIndex = state == undefined
      ? 0
      : state.startDateIndex || 0;
    const prevStartDateIndex = startDateIndex - 5;
    const nextStartDateIndex = startDateIndex + 5;

    return (
      <div className="d-flex justify-content-between">
        <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
        {(state == undefined || state.isLoading) && 
          <span>Loading...</span>
        }
        <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
      </div>
    );
  }

  return (
    <React.Fragment>
      <h1 id="tabelLabel">Weather forecast</h1>
      <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
      {renderForecastsTable()}
      {renderPagination()}
    </React.Fragment>
  );
}

export default FetchData;
