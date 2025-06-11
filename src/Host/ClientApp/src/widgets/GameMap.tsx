import * as React from 'react';
import { useMemo } from 'react';
import { ImageOverlay, MapContainer, GeoJSON } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import './gameMap.css';
import mapImage from '../assets/images/worldmap/map.jpg';
import mapData from '../assets/geoJson/mapGeoJson.json';
import type { Feature, FeatureCollection } from 'geojson';
import { useIndexQuery } from '../entities/MapData';
import ErrorField from '../shared/ErrorField';
import { useNavigate } from 'react-router-dom';

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

const GameMap: React.FC = () => {
    const { data, error } = useIndexQuery();
    const navigate = useNavigate();

    const geoJsonData = useMemo(() => {
        if (!data) return mapData as FeatureCollection;

        return {
            ...mapData,
            features: (mapData as FeatureCollection).features.map(feature => ({
                ...feature,
                properties: {
                    ...feature.properties,
                    colorStr: data[feature.properties?.id]?.colorStr,
                    mapElement: data[feature.properties?.id]
                }
            }))
        } as FeatureCollection;
    }, [data]);

    const onEachFeature = (feature: Feature, layer: L.Layer) => {
        layer.on({
            mouseover: (e) => {
                const hoveredLayer = e.target;
                hoveredLayer.setStyle({
                    ...geoJsonStyle,
                    fillColor: feature.properties?.colorStr || 'rgba(120, 120, 120, 0.7)',
                    ...hoverStyle
                });
                hoveredLayer.bringToFront();
            },
            mouseout: (e) => {
                e.target.setStyle({
                    ...geoJsonStyle,
                    fillColor: feature.properties?.colorStr || 'rgba(120, 120, 120, 0.7)'
                });
            },
            click: () => {
                navigate(`/app/province/${feature.properties?.id}`)

            }
        });
    };

    const calculateOptimalZoom = () : number => {
        if (typeof window === 'undefined') return -2;
        
        const isMobile = window.innerWidth < 768;
        return isMobile ? -2 : -1; 
    };

    const renderMap = () => {
        return (
            <MapContainer
                crs={L.CRS.Simple}
                bounds={[[0, 0], [2076, 1839]]}
                minZoom={calculateOptimalZoom()}
                zoom={0}
                scrollWheelZoom={true}
            >
                <ImageOverlay
                    url={mapImage}
                    bounds={[[0, 0], [2076, 1839]]}
                />

                <GeoJSON
                    key={JSON.stringify(data)}
                    data={geoJsonData}
                    style={(feature) => ({
                        ...geoJsonStyle,
                        fillColor: feature?.properties?.colorStr || 'rgba(120, 120, 120, 0.7)'
                    })}
                    onEachFeature={onEachFeature}
                />
            </MapContainer>
        )
    }

    return (
        <>
            {error && <ErrorField title='Ошибка' error={error} />}
            {renderMap()}
        </>
    )


};

export default GameMap;