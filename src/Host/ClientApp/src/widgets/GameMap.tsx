import * as React from 'react';
import { ImageOverlay, MapContainer, GeoJSON } from 'react-leaflet';

import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import './gameMap.css';
import mapImage from '../assets/images/worldmap/map.jpg'
import mapData from '../assets/geoJson/mapGeoJson.json'
import type { Feature, FeatureCollection } from 'geojson';

const geoJsonStyle = {
    fillOpacity: 1,
    weight: 0,
    color: '#4a4a4a',
    opacity: 0.3,
    className: 'province-area'
};

const hoverStyle = {
    fillOpacity: 0.6,
    weight: 1,
};

export interface GameMapProps {
    mapElements: GameMapElement[]
}

export interface GameMapElement {
    id: string,
    name: string,
    description: string,
    color: string,
}

const GameMap: React.FC<GameMapProps> = ({ mapElements }) => {

    const onEachFeature = (feature: any, layer: L.Layer) => {
        layer.on({
            mouseover: (e) => {
                const hoveredLayer = e.target;
                hoveredLayer.setStyle({
                    ...getStyle(feature),
                    ...hoverStyle
                });
            },
            mouseout: (e) => {
                const hoveredLayer = e.target;
                hoveredLayer.setStyle({
                    ...getStyle(feature)
                });
            }
        });
    };

    const getFactionColor = (id: string): string => {
        return mapElements.find(e => e.id == id)?.color ?? 'rgba(120, 120, 120, 0.7)'
    }

    const getStyle = (feature: Feature | undefined) => ({
        ...geoJsonStyle,
        fillColor: getFactionColor(feature?.properties?.["id"]),
    });

    const renderMap = () => {
        const geoJsonData = mapData as FeatureCollection;

        return (
            <MapContainer
                crs={L.CRS.Simple}
                bounds={[[0, 0], [2076, 1839]]}
                minZoom={-2}
                zoom={0}
                scrollWheelZoom={true}>

                <ImageOverlay
                    url={mapImage}
                    bounds={[[0, 0], [2076, 1839]]}
                />

                <GeoJSON
                    data={geoJsonData}
                    style={(feature) => getStyle(feature)}
                    onEachFeature={onEachFeature}
                />
            </MapContainer>
        )
    }

    const render = () => {
        return (
            <>
                {renderMap()}
            </>
        )
    }

    return render();
};

export default GameMap;